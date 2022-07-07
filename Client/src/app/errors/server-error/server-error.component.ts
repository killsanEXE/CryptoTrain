import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ServerErrorComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
