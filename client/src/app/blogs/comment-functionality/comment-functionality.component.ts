import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-comment-functionality',
  templateUrl: './comment-functionality.component.html',
  styleUrls: ['./comment-functionality.component.css']
})
export class CommentFunctionalityComponent implements OnInit {
  @Input() id: number;

  constructor() { }

  ngOnInit(): void {
  }

}
