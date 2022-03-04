export interface Blog {
    id: number;
    title: string;
    blogContent: string;
    picture: string;
    publishedOn: Date;
    updatedOn: Date;
    username: string;
}

export interface BlogCreateEdit {
    id: number;
    title: string;
    blogContent: string;
    picture: File;
}

export class BlogClass {

    constructor(
        public id: number,
        public title: string,
        public content: string,
        public applicationUserId: number,
        public username: string,
        public publishDate: Date,
        public updateDate: Date,
        public deleteConfirm: boolean = false,
        public picture: string
    ) {}

}

export class BlogCreateEditClass {

    constructor(
        public id: number,
        public title: string,
        public content: string,
        public picture: string
    ) {}

}

