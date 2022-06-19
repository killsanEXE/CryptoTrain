import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss', './nav.style.scss']
})
export class NavComponent implements OnInit {

  @ViewChild("menuBtn") menuBtn: any;
  menuOpen = false;

  constructor() { }

  ngOnInit(): void {
  }

  changeMenu = () => this.menuOpen ? this.menuOpen = false : this.menuOpen = true

}
