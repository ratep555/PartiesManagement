import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Item } from '../shared/models/item';
import { UserParams } from '../shared/models/myparams';
import { ItemsService } from './items.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  items: Item[];
  userParams: UserParams;
  totalCount: number;

  constructor(private itemsService: ItemsService,
              private  router: Router) {
    this.userParams = this.itemsService.getUserParams();
     }

  ngOnInit(): void {
    this.getItems();
  }

  getItems() {
    this.itemsService.setUserParams(this.userParams);
    this.itemsService.getItems(this.userParams)
    .subscribe(response => {
      this.items = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getItems();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.itemsService.resetUserParams();
    this.getItems();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.itemsService.setUserParams(this.userParams);
      this.getItems();
    }
}

}
