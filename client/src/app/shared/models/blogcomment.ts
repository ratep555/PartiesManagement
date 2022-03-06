export class BlogCommentClass {

    constructor(
        public id: number,
        public blogId: number,
        public commentContent: string,
        public applicationUserId: number,
        public username: string,
        public publishedOn: Date,
        public updatedOn: Date,
        public parentBlogCommentId?: number

    ) {}
}

export class BlogCommentCreateEditClass {

    constructor(
        public id: number,
        public blogId: number,
        public commentContent: string,
        public parentBlogCommentId?: number
    ) {}
}

export class BlogCommentClientOnlyClass {

        constructor(
            public parentBlogCommentId: number,
            public blogId: number,
            public id: number,
            public commentContent: string,
            public username: string,
            public publishedOn: Date,
            public updatedOn: Date,
            public isEditable: boolean = false,
            public deleteConfirm: boolean = false,
            public isReplying: boolean = false,
            public comments: BlogCommentClientOnlyClass[]
        ) {}
    }

