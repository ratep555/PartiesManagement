import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket, BasketItem } from 'src/app/shared/models/basket';
import { Item } from 'src/app/shared/models/item';
import { WebshopService } from '../webshop.service';

@Component({
  selector: 'app-item-list-id',
  templateUrl: './item-list-id.component.html',
  styleUrls: ['./item-list-id.component.css']
})
export class ItemListIdComponent implements OnInit {
  basket$: Observable<Basket>;
  @Output() decrease: EventEmitter<BasketItem> = new EventEmitter<BasketItem>();
  @Output() increase: EventEmitter<BasketItem> = new EventEmitter<BasketItem>();
  @Output() remove: EventEmitter<BasketItem> = new EventEmitter<BasketItem>();
  @Input() isBasket = true;
  @Input() isOrder = false;
  @Input() item: Item;
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

  increaseBasketItemQuantity(item: BasketItem) {
    if (this.item.stockQuantity > 1) {
      this.quantity++;
      this.item.stockQuantity--;
      this.webshopService.decreaseStockQuantity1(item.id, 1).subscribe(() => {
      });
      this.increase.emit(item);
    }
  }

  decreaseBasketItemQuantity(item: BasketItem) {
    if (this.quantity > 1) {
      this.quantity--;
      this.item.stockQuantity++;
      this.webshopService.increaseStockQuantity1(item.id, 1).subscribe(() => {
      });
      this.decrease.emit(item);
    }
  }

  removingItemFromBasket(item: BasketItem) {
    this.webshopService.increaseStockQuantity1(item.id, item.quantity).subscribe(() => {
    });
    this.remove.emit(item);
  }
}










