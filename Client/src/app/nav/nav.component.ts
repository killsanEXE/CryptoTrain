import { Component, OnInit, ViewChild } from '@angular/core';
import { BusyService } from '../_services/busy.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss', './nav.style.scss']
})
export class NavComponent implements OnInit {

  @ViewChild("menuBtn") menuBtn: any;
  menuOpen = false;

  constructor(public busyService: BusyService) { }

  ngOnInit(): void {
  }

  changeMenu = () => this.menuOpen ? this.menuOpen = false : this.menuOpen = true
  closeMenu = () => this.menuOpen = false;

}
