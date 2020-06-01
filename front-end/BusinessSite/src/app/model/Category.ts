export class Category implements INavigation {
    public id: number;

    public code: string;
    public displayName: string;
    public imageUrl: string;

    public isEverything: boolean;

    public version: number;

    // [only-front], url for nagivation
    public url: string;
  }