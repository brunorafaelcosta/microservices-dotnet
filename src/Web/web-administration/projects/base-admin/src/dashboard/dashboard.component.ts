import { Component, OnInit } from '@angular/core';

import { LayoutService } from 'shared';

@Component({
  selector: 'dashboard-root',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.sass'],
})
export class DashboardComponent implements OnInit {
  private title: string = 'Dashboard';

  constructor(private layoutService: LayoutService) {
  }

  ngOnInit(): void {
    this.layoutService.setTitle(this.title);
  }
}
