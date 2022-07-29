import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { take } from 'rxjs';
import { CryptoModel } from 'src/app/_models/cryptoModel';
import { CryptoService } from 'src/app/_services/crypto.service';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';

@Component({
  selector: 'app-crypto',
  templateUrl: './crypto.component.html',
  styleUrls: ['./crypto.component.scss']
})
export class CryptoComponent implements AfterViewInit {

  history: CryptoModel[] = [];
  data: number[] = [];
  labels: string[] = [];
  chart: Chart = null;

  @ViewChild("mainChart", { static: false }) canvas: ElementRef<HTMLCanvasElement>;
  @ViewChild("innerWrapper", { static: false }) innerWrapper: ElementRef<HTMLDivElement>;

  constructor(private cryptoService: CryptoService) { }
  ngAfterViewInit(): void {
    Chart.register(...registerables);
    this.loadCryptoHistory(this.cryptoService.getUserParams());
  }

  loadCryptoHistory(userParams: UserParams) {
    this.cryptoService.getBTCHistory(userParams).subscribe(f => {
      this.cryptoService.crypto$.pipe(take(1)).subscribe(history => {
        this.data.splice(0, this.data.length);
        this.labels.splice(0, this.labels.length);
        history.forEach(btc => {
          this.data.unshift(btc.price);
          this.labels.unshift(btc.date.toString().slice(0, 10));
        })

        if (this.chart === null) { this.generateChart(); }
        else{
          this.innerWrapper.nativeElement.style.width = 
            `${this.innerWrapper.nativeElement.clientWidth + 400}px`; 
          this.chart.update();
        } 
      });
    });
  }


  generateChart(){
    this.chart = new Chart(this.canvas.nativeElement.getContext("2d"), {
      type: 'line',
      data: {
        labels: this.labels,
        datasets: [{
          label: "BTC Price",
          data: this.data,
          fill: true,
        }]
      },
      options: {
        maintainAspectRatio: false,
        responsive: true,
        animation: false,
        scales: {
          x: {
            offset: false,
            // min: 0,
            // max: 2
          },
          y: {
            position: {
              x: 0,
            }
          }
        }
      }
    });
  }

  loadMore(){
    let params = this.cryptoService.getUserParams();
    params.pageNumber++;
    this.cryptoService.setUserParams(params);
    this.loadCryptoHistory(params);
    }
}
