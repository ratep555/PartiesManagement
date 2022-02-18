import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Discount } from 'src/app/shared/models/discount';
import { UserParams } from 'src/app/shared/models/myparams';
import { DiscountsService } from './discounts.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { ToastrService } from 'ngx-toastr';

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
              private  router: Router,
              private toastr: ToastrService) {
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

onDelete(id: number) {
  Swal.fire({
    title: 'Are you sure want to delete this record?',
    text: 'You will not be able to recover it afterwards!',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, delete it!',
    confirmButtonColor: '#DD6B55',
    cancelButtonText: 'No, keep it'
  }).then((result) => {
    if (result.value) {
        this.discountsService.deleteDiscount(id)
    .subscribe(
      res => {
        this.getDiscounts();
        this.toastr.error('Deleted successfully!');
      }, err => { console.log(err);
       });
  }
});
}

}
