import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { OrderStatus } from 'src/app/shared/models/enums';

@Component({
  selector: 'app-enum-attempt',
  templateUrl: './enum-attempt.component.html',
  styleUrls: ['./enum-attempt.component.css']
})
export class EnumAttemptComponent implements OnInit {
  orderStatus = OrderStatus;
  enumKeys = [];

  constructor(private fb: FormBuilder) {
    this.enumKeys = Object.keys(this.orderStatus);
   }

  ngOnInit(): void {
  }

}
