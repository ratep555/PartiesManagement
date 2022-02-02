import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { Item } from 'src/app/shared/models/item';
import { WebshopService } from '../webshop.service';

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
  }

  increaseQuantity() {
    this.quantity++;
  }

  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

}
