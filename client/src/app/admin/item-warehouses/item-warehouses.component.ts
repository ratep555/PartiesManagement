import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { ToastrService } from 'ngx-toastr';
import { ItemWarehouse } from 'src/app/shared/models/itemWarehouses';
import { UserParams } from 'src/app/shared/models/myparams';
import { ItemWarehousesService } from './item-warehouses.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-item-warehouses',
  templateUrl: './item-warehouses.component.html',
  styleUrls: ['./item-warehouses.component.css']
})
export class ItemWarehousesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  itemWarehouses: ItemWarehouse[];
  userParams: UserParams;
  totalCount: number;

  constructor(private itemWarehousesService: ItemWarehousesService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.itemWarehousesService.getUserParams();
     }

  ngOnInit(): void {
    this.getItemWarehouses();
  }

  getItemWarehouses() {
    this.itemWarehousesService.setUserParams(this.userParams);
    this.itemWarehousesService.getItemWarehouses(this.userParams)
    .subscribe(response => {
      this.itemWarehouses = response.data;
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
    this.getItemWarehouses();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.itemWarehousesService.resetUserParams();
    this.getItemWarehouses();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.itemWarehousesService.setUserParams(this.userParams);
      this.getItemWarehouses();
    }
}

/* onDelete(id: number) {
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
} */

}
