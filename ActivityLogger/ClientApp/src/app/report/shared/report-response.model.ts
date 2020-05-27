import { ReportItem } from "./report-item.model";

export interface ReportResponse {
    queriedDate: string,
    data: ReportItem[]
}