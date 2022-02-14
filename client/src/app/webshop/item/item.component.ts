import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Item } from 'src/app/shared/models/item';
import { WebshopService } from '../webshop.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  @Input() item: Item;

  constructor(private basketService: BasketService, private webshopService: WebshopService) { }

  ngOnInit(): void {
  }

  addingItemToBasket() {
    this.basketService.addingItemToBasket(this.item);
    this.webshopService.decreaseStockQuantity1(this.item.id, 1).subscribe(() => {
    });
  }

}
