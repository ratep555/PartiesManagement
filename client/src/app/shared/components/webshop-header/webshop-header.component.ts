import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-webshop-header',
  templateUrl: './webshop-header.component.html',
  styleUrls: ['./webshop-header.component.css']
})
export class WebshopHeaderComponent implements OnInit {
  @Input() pageNumber: number;
  @Input() pageSize: number;
  @Input() totalCount: number;

  constructor() { }

  ngOnInit(): void {
  }

}
