import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule} from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PagerComponent } from './components/pager/pager.component';
import { RouterModule } from '@angular/router';
import { WebshopHeaderComponent } from './components/webshop-header/webshop-header.component';
import { OrderSumComponent } from './components/order-sum/order-sum.component';
import { TextInputComponent } from './components/text-input/text-input.component';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { CdkStepperComponent } from './components/cdk-stepper/cdk-stepper.component';
import { BasketReviewComponent } from './components/basket-review/basket-review.component';
import { ImgInputComponent } from './components/img-input/img-input.component';
import { MultipleSelectorComponent } from './components/multiple-selector/multiple-selector.component';
import { RatingComponent } from './components/rating/rating.component';
import { LeafletModule} from '@asymmetrik/ngx-leaflet';
import 'leaflet/dist/images/marker-icon-2x.png';
import { MapComponent } from './components/map/map.component';


@NgModule({
  declarations: [
    PagerComponent,
    WebshopHeaderComponent,
    OrderSumComponent,
    TextInputComponent,
    CdkStepperComponent,
    BasketReviewComponent,
    ImgInputComponent,
    MultipleSelectorComponent,
    RatingComponent,
    MapComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    BsDropdownModule.forRoot(),
    CollapseModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    ModalModule.forRoot(),
    CdkStepperModule,
    LeafletModule
  ],
  exports: [
    BsDropdownModule,
    CollapseModule,
    ReactiveFormsModule,
    FormsModule,
    TabsModule,
    BsDatepickerModule,
    PaginationModule,
    ModalModule,
    PagerComponent,
    WebshopHeaderComponent,
    OrderSumComponent,
    TextInputComponent,
    CdkStepperModule,
    CdkStepperComponent,
    BasketReviewComponent,
    ImgInputComponent,
    MultipleSelectorComponent,
    RatingComponent,
    LeafletModule
    ]
})
export class SharedModule { }
