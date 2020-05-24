import { Category } from "./category.model";

export interface Activity {
    id: number;
    category: Category;
    startDateTime: string;
    endDateTime: string;
    comment: string;
}