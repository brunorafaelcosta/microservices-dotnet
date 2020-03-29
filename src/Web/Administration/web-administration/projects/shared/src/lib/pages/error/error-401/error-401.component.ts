import { Component, OnInit } from '@angular/core';

import { LayoutService } from '../../../layout';

@Component({
  selector: 'error-401-root',
  templateUrl: './error-401.component.html',
  styleUrls: ['./error-401.component.sass'],
})
export class Error401Component implements OnInit {
  private title: string = 'Error 401';

  constructor(private layoutService: LayoutService) {
  }

  ngOnInit(): void {
    this.layoutService.setTitle(this.title);
  }
}
