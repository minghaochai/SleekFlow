import { IconButton as MuiIconButton } from '@mui/material';

import { IconButtonProps } from './types';

export function IconButton(props: IconButtonProps) {
  const { children, disabled, onClick } = props;
  return (
    <MuiIconButton disabled={disabled} onClick={onClick}>
      {children}
    </MuiIconButton>
  );
}

export default IconButton;
