import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ItemWarehouse } from 'src/app/shared/models/itemWarehouses';
import { ItemWarehousesService } from '../item-warehouses.service';

@Component({
  selector: 'app-add-item-warehouse',
  templateUrl: './add-item-warehouse.component.html',
  styleUrls: ['./add-item-warehouse.component.css']
})
export class AddItemWarehouseComponent implements OnInit {
  itemWarehouseForm: FormGroup;
  model: ItemWarehouse;
  itemsList = [];
  warehousesList = [];

  constructor(public itemWarehousesService: ItemWarehousesService,
              private router: Router,
              private fb: FormBuilder) { }

  ngOnInit(): void {
    this.itemWarehousesService.getItems()
    .subscribe(res => this.itemsList = res as []);

    this.itemWarehousesService.getWarehouses()
    .subscribe(res => this.warehousesList = res as []);

    this.itemWarehouseForm = this.fb.group({
      itemId: [null],
      warehouseId: [null],
     });
  }

  onSubmit() {
    if (this.itemWarehouseForm.invalid) {
        return;
    }
    this.createItemWarehouse();
  }

  private createItemWarehouse() {
    this.itemWarehousesService.createItemWarehouse(this.itemWarehouseForm.value).subscribe(() => {
      this.router.navigateByUrl('itemwarehouses');
    },
    error => {
      console.log(error);
    });
  }

}
