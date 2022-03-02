import { Component, Input, OnInit } from '@angular/core';
import { ServiceIncluded } from 'src/app/shared/models/birthdays/serviceincluded';

@Component({
  selector: 'app-service-included',
  templateUrl: './service-included.component.html',
  styleUrls: ['./service-included.component.css']
})
export class ServiceIncludedComponent implements OnInit {
  @Input() serviceIncluded: ServiceIncluded;

  constructor() { }

  ngOnInit(): void {
  }

}
