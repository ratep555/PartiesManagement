import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Item } from '../shared/models/item';
import { MyParams } from '../shared/models/myparams';
import { WebshopService } from './webshop.service';

@Component({
  selector: 'app-webshop',
  templateUrl: './webshop.component.html',
  styleUrls: ['./webshop.component.css']
})
export class WebshopComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  items: Item[];
  myParams: MyParams;
  totalCount: number;

  constructor(private webshopService: WebshopService) {
    this.myParams = this.webshopService.getMyParams();
   }

  ngOnInit(): void {
    this.getItems();
  }

  getItems() {
    this.webshopService.getItems().subscribe(response => {
      this.items = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any) {
    const params = this.webshopService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.webshopService.setMyParams(params);
      this.getItems();
    }
  }

  onSearch() {
    const params = this.webshopService.getMyParams();
    params.query = this.searchTerm.nativeElement.value;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getItems();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.webshopService.setMyParams(this.myParams);
    this.getItems();
  }


}

















