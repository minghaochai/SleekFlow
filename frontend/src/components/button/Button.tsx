import { Button as MuiButton, styled } from '@mui/material';
import { unstable_extendSxProp as extendSxProp } from '@mui/system';

import { ButtonProps } from './types';

const StyledMuiButton = styled(MuiButton)({});

export function Button(props: ButtonProps) {
  const { children, onClick, sx } = props;
  const { sx: customXs } = extendSxProp({ sx });
  return (
    <StyledMuiButton sx={customXs} onClick={onClick}>
      {children}
    </StyledMuiButton>
  );
}

export default Button;
