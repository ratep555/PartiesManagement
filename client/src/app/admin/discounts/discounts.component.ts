import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Discount } from 'src/app/shared/models/discount';
import { UserParams } from 'src/app/shared/models/myparams';
import { DiscountsService } from './discounts.service';

@Component({
  selector: 'app-discounts',
  templateUrl: './discounts.component.html',
  styleUrls: ['./discounts.component.css']
})
export class DiscountsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  discounts: Discount[];
  userParams: UserParams;
  totalCount: number;

  constructor(private discountsService: DiscountsService,
              private  router: Router) {
    this.userParams = this.discountsService.getUserParams();
     }

  ngOnInit(): void {
    this.getDiscounts();
  }

  getDiscounts() {
    this.discountsService.setUserParams(this.userParams);
    this.discountsService.getDiscounts(this.userParams)
    .subscribe(response => {
      this.discounts = response.data;
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
    this.getDiscounts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.discountsService.resetUserParams();
    this.getDiscounts();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.discountsService.setUserParams(this.userParams);
      this.getDiscounts();
    }
}

}
