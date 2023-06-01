import { Dialog as MuiDialog } from '@mui/material';

import { DialogProps } from './types';

export function Dialog(props: DialogProps) {
  const { open, handleClose } = props;
  return (
    <MuiDialog open={open} onClose={handleClose}>
      {props.children}
    </MuiDialog>
  );
}

export default Dialog;
