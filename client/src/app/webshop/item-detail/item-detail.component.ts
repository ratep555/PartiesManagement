import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ChildActivationStart } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { Item } from 'src/app/shared/models/item';
import { WebshopService } from '../webshop.service';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-item-detail',
  templateUrl: './item-detail.component.html',
  styleUrls: ['./item-detail.component.css']
})
export class ItemDetailComponent implements OnInit {
  item: Item;
  quantity = 1;

  constructor(private webshopService: WebshopService,
              private activatedRoute: ActivatedRoute,
              private basketService: BasketService) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    this.webshopService.getItem(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(item => {
      this.item = item;
    }, error => {
      console.log(error);
    });
  }

  addingItemToBasket() {
    this.basketService.addingItemToBasket(this.item, this.quantity);
    this.webshopService.decreaseStockQuantity1(this.item.id, this.quantity).subscribe(() => {
      this.loadProduct();
    })
  }

  increaseQuantity1() {
      this.quantity++;
  }

  increaseQuantity() {
    if (this.item.stockQuantity > 1) {
      this.quantity++;
      this.item.stockQuantity--;
    }
  }

  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
      this.item.stockQuantity++;
    }
  }

  onRating(rate: number){
    this.webshopService.rate(this.item.id, rate).subscribe(() => {
     Swal.fire('Success', 'Your vote has been received', 'success');
     this.loadProduct();
   });
 }

}
