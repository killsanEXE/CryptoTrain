import { Component, OnInit } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { CryptoModel } from 'src/app/_models/cryptoModel';
import { CryptoService } from 'src/app/_services/crypto.service';

@Component({
  selector: 'app-crypto',
  templateUrl: './crypto.component.html',
  styleUrls: ['./crypto.component.scss']
})
export class CryptoComponent implements OnInit {

  history: CryptoModel[] = [];
  data: number[] = [];
  labels: string[] = [];

  constructor(private cryptoService: CryptoService) { }

  ngOnInit(): void {
    Chart.register(...registerables);
    this.cryptoService.getBTCHistory().subscribe(history => {
      this.history = history;
      history.forEach(f => {
        this.data.unshift(f.price);
        let date = f.date.toString();
        this.labels.unshift(date.slice(0, 10))
      })
      this.generateChart();
    });
  }

  generateChart(){
    const myChart = new Chart("myChart", {
      type: 'line',
      data: {
        labels: this.labels,
        datasets: [{
          label: 'BTC Price',
          data: this.data,
          tension: 2
        }]
      },
      options: {
        animation: false,
        
      }
    });
  }
}
