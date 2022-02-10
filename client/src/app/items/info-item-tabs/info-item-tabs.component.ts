import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Item } from 'src/app/shared/models/item';
import Swal from 'sweetalert2';
import { ItemsService } from '../items.service';


@Component({
  selector: 'app-info-item-tabs',
  templateUrl: './info-item-tabs.component.html',
  styleUrls: ['./info-item-tabs.component.css']
})
export class InfoItemTabsComponent implements OnInit {
  item: Item;

  constructor(private itemsService: ItemsService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getItemById();
  }

  getItemById() {
    return this.itemsService.getItemById(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe(response => {
    this.item = response;
    }, error => {
    console.log(error);
    });
}

}
