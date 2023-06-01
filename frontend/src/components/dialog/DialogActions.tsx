import { DialogActions as MuiDialogActions } from '@mui/material';

import { DialogActionsProps } from './types';

export function DialogActions(props: DialogActionsProps) {
  return <MuiDialogActions>{props.children}</MuiDialogActions>;
}

export default DialogActions;
