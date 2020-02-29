import { Component, OnInit, Input, Output } from '@angular/core';
import { EventEmitter } from 'events';

@Component({
  selector: 'action-item-card',
  templateUrl: './action-item-card.component.html',
  styleUrls: ['./action-item-card.component.scss'],
})
export class ActionItemCardComponent {
  @Input() headerText: string;
  @Input() time: Date;
  @Input() date: Date;
  @Input() bgCss: string;

  @Output() clicked = new EventEmitter();

  public onItemClicked($event: Event) {
    this.clicked.emit(null);
  }
}
