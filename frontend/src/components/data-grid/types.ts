import { GridColDef, GridSortDirection } from '@mui/x-data-grid';

export type DataGridProps = {
  columns: GridColDef[];
  rows: any;
} & PaginationProps &
  SortProps;

interface PaginationProps {
  currentPage?: number;
  pageSize?: number;
  totalItems?: number;
  handlePaginationModelChange?: (page: number, pageSize: number) => void;
}

interface SortProps {
  handleSortModelChange?: (
    field: string | undefined,
    sort: GridSortDirectionModel,
  ) => void;
}

export type GridSortDirectionModel = GridSortDirection;
