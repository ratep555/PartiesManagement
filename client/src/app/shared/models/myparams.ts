import { User } from './user';

export class MyParams {
    manufacturerId = 0;
    tagId = 0;
    categoryId = 0;
    query: string;
    page = 1;
    pageCount = 12;
}

export class UserParams {
    manufacturerId = 0;
    tagId = 0;
    categoryId = 0;
    query: string;
    page = 1;
    pageCount = 12;
    displayName: string;

    constructor(user: User) {
        this.displayName = user.displayName;
   }
}
