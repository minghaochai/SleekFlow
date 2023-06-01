import { styled } from '@mui/material';
import {
  DataGrid as BuiDataGrid,
  GridCallbackDetails,
  GridPaginationModel,
  GridSortModel,
} from '@mui/x-data-grid';
import { useState } from 'react';

import { DataGridProps } from './types';

export function DataGrid(props: DataGridProps) {
  const {
    columns,
    rows,
    pageSize,
    currentPage,
    totalItems,
    handlePaginationModelChange,
    handleSortModelChange,
  } = props;
  const [sortModel, setSortModel] = useState<GridSortModel>();

  const StyledMuiDataGrid = styled(BuiDataGrid)({
    margin: 'auto',
  });

  const onPaginationModelChange = (
    model: GridPaginationModel,
    _details: GridCallbackDetails,
  ) => {
    if (handlePaginationModelChange)
      handlePaginationModelChange(model.page, model.pageSize);
  };

  const onSortModelChange = (sortModel: GridSortModel) => {
    if (handleSortModelChange) {
      if (sortModel.length > 0) {
        handleSortModelChange(
          sortModel[0].field,
          sortModel[0].sort ?? undefined,
        );
      } else {
        handleSortModelChange(undefined, undefined);
      }
    }
    setSortModel(sortModel);
  };

  return (
    <StyledMuiDataGrid
      rows={rows}
      columns={columns}
      rowCount={totalItems}
      pageSizeOptions={[5]}
      paginationModel={{
        page: currentPage! - 1,
        pageSize: pageSize!,
      }}
      paginationMode='server'
      onPaginationModelChange={onPaginationModelChange}
      sortingMode='server'
      onSortModelChange={onSortModelChange}
      sortModel={sortModel}
      disableRowSelectionOnClick
    />
  );
}

export default DataGrid;
