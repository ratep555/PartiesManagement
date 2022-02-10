import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.css']
})
export class RatingComponent implements OnInit {
  @Input() maxRating = 5;
  @Input() selectedRate = 0;
  @Output() nonRating: EventEmitter<number> = new EventEmitter<number>();
  previousRate = 0;
  maxRatingArr = [];

  stars: number[] = [1, 2, 3, 4, 5];

  constructor() {}

  ngOnInit(): void {
    this.maxRatingArr = Array(this.maxRating).fill(0);
  }


  handleMouseEnter(index: number){
    this.selectedRate = index + 1;
  }

  handleMouseLeave(){
    if (this.previousRate !== 0){
      this.selectedRate = this.previousRate;
    } else{
      this.selectedRate = 0;
    }
  }

  rate(index: number){
      this.selectedRate = index + 1;
      this.previousRate = this.selectedRate;
      this.nonRating.emit(this.selectedRate);
  }
}