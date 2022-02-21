import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { ItemWarehouse } from 'src/app/shared/models/itemWarehouses';
import { ItemWarehousesService } from '../item-warehouses.service';

@Component({
  selector: 'app-edit-item-warehouse',
  templateUrl: './edit-item-warehouse.component.html',
  styleUrls: ['./edit-item-warehouse.component.css']
})
export class EditItemWarehouseComponent implements OnInit {
  itemWarehouseForm: FormGroup;
  itemWarehouse: ItemWarehouse;
  itemsList = [];
  warehousesList = [];
  id: number;
  warehouseid: number;

  constructor(public itemWarehouseService: ItemWarehousesService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];
    this.warehouseid = this.activatedRoute.snapshot.params['warehouseid'];

    this.itemWarehouseService.getItems()
    .subscribe(res => this.itemsList = res as []);

    this.itemWarehouseService.getWarehouses()
    .subscribe(res => this.warehousesList = res as []);

    this.itemWarehouseForm = this.fb.group({
      itemId: [this.id],
      warehouseId: [this.warehouseid],
      stockQuantity: ['', [Validators.required]],
      reservedQuantity: ['', [Validators.required]]
     });

    this.itemWarehouseService.getItemWarehouseByItemIdAndWarehouseId(this.id, this.warehouseid)
    .pipe(first())
    .subscribe(x => this.itemWarehouseForm.patchValue(x));
  }

  onSubmit() {
    if (this.itemWarehouseForm.invalid) {
        return;
    }
    this.updateItemWarehouse();
  }

  private updateItemWarehouse() {
    this.itemWarehouseService.updateItemWarehouse(this.id, this.warehouseid, this.itemWarehouseForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('itemwarehouses');
          }, error => {
            console.log(error);
          });
        }

}
