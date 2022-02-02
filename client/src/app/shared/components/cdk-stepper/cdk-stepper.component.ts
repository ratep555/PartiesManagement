import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-cdk-stepper',
  templateUrl: './cdk-stepper.component.html',
  styleUrls: ['./cdk-stepper.component.css'],
  providers: [{provide: CdkStepper, useExisting: CdkStepperComponent}]

})
export class CdkStepperComponent extends CdkStepper implements OnInit {
  @Input() linearOptionSelected: boolean;

  ngOnInit() {
    this.linear = this.linearOptionSelected;
  }

  onClick(index: number) {
    this.selectedIndex = index;
  }

}
