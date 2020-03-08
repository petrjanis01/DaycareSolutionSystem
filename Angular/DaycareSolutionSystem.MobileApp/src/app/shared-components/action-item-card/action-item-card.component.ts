import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'action-item-card',
  templateUrl: './action-item-card.component.html',
  styleUrls: ['./action-item-card.component.scss'],
})
export class ActionItemCardComponent {
  @Input() headerText: string;
  @Input() fromTime: Date;
  @Input() untilTime: Date;
  @Input() date: Date;
  @Input() bgCss: string;

  @Output() clicked = new EventEmitter();

  public onItemClicked($event: Event) {
    this.clicked.emit(null);
  }
}
