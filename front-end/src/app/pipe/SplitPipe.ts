import { PipeTransform, Pipe } from '@angular/core';

@Pipe({
  name: 'split',
})

export class SplitPipe implements PipeTransform
{
  transform(value: any, args?: any): any
  {
    return value.split('');
  }
}
