import { render, screen } from '@testing-library/react';

import SvgIcon from './SvgIcon';

describe('SvgIcon component', () => {
  test('rendered SvgIcon', () => {
    //arrange
    render(<SvgIcon>Icon</SvgIcon>);

    //act
    const svgIconElement = screen.getByText('Icon');

    //assert
    expect(svgIconElement).toBeInTheDocument();
  });
});
