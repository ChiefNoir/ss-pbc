import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appOnlyInt]',
})
  
export class OnlyIntDirective {
  private navigationKeys = [
    'Backspace',
    'Delete',
    'Tab',
    'Escape',
    'Enter',
    'Home',
    'End',
    'ArrowLeft',
    'ArrowRight',
    'Clear',
    'Copy',
    'Paste',
  ];

  private numberKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];

  constructor() {}

  @HostListener('keydown', ['$event'])
  onKeyDown(e: KeyboardEvent) {
    if (this.navigationKeys.indexOf(e.key) > -1) {
      return;
    }

    if (this.numberKeys.indexOf(e.key) > -1) {
      return;
    }

    e.preventDefault();
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent) {
    event.preventDefault();
  }

  @HostListener('drop', ['$event'])
  onDrop(event: DragEvent) {
    event.preventDefault();
  }
}
