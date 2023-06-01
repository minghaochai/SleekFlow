import { SvgIcon as MuiSvgIcon } from '@mui/material';

import { SvgIconProps } from './types';

export function SvgIcon({ height, width, ...props }: SvgIconProps) {
  return (
    <MuiSvgIcon
      sx={[
        height !== undefined && { height },
        width !== undefined && { width },
      ]}
      {...props}
    />
  );
}

export default SvgIcon;
