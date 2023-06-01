import { DialogContent as MuiDialogContent } from '@mui/material';

import { DialogContentProps } from './types';

export function DialogContent(props: DialogContentProps) {
  return <MuiDialogContent>{props.children}</MuiDialogContent>;
}

export default DialogContent;
