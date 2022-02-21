import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ChildActivationStart } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { Item } from 'src/app/shared/models/item';
import { WebshopService } from '../webshop.service';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';


@Component({
  selector: 'app-item-detail',
  templateUrl: './item-detail.component.html',
  styleUrls: ['./item-detail.component.css']
})
export class ItemDetailComponent implements OnInit {
  item: Item;
  quantity = 1;
  result: number;

  constructor(private webshopService: WebshopService,
              private activatedRoute: ActivatedRoute,
              private basketService: BasketService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadProduct();
   // this.result = this.item.stockQuantity - this.quantity;
  }

  loadProduct() {
    this.webshopService.getItem(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(item => {
      this.item = item;
    }, error => {
      console.log(error);
    });
  }

  addingItemToBasket() {
    const result = this.item.stockQuantity - this.quantity;
    if (result >= 0) {
      this.basketService.addingItemToBasket(this.item, this.quantity);
    } else {
      this.toastr.error('Insuficient quantity!');
    }
    this.webshopService.decreaseStockQuantity1(this.item.id, this.quantity).subscribe(() => {
      this.loadProduct();
    });

  }

  addingItemToBasket1() {
    this.basketService.addingItemToBasket1(this.item, this.quantity, this.item.stockQuantity);
    this.webshopService.decreaseStockQuantity1(this.item.id, this.quantity).subscribe(() => {
      this.loadProduct();
    });
  }

  increaseQuantity1() {
      this.quantity++;
  }

  increaseQuantity() {
    if (this.item.stockQuantity > 1 && this.quantity < this.item.stockQuantity) {
      this.quantity++;
     // this.item.stockQuantity--;
    }
  }

  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
   //   this.item.stockQuantity++;
    }
  }

  onRating(rate: number){
    this.webshopService.rate(this.item.id, rate).subscribe(() => {
     Swal.fire('Success', 'Your vote has been received', 'success');
     this.loadProduct();
   });
 }

}
