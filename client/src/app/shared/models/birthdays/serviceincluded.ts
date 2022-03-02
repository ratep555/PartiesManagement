export interface ServiceIncluded {
    id: number;
    name: string;
    description: string;
    picture: string;
    videoClip: string;
}

export interface ServiceIncludedCreateEdit {
    id: number;
    name: string;
    description: string;
    picture: File;
    videoClip: string;
}
