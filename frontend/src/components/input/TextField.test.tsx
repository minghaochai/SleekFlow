import { render, screen } from '@testing-library/react';

import TextField from './TextField';

describe('TextField component', () => {
  test('rendered TextField', () => {
    //arrange
    render(
      <TextField
        autoFocus
        id='name'
        label={'textfield'}
        fullWidth
        variant='standard'
        defaultValue={'default'}
      />,
    );

    //act
    const textFieldElement = screen.getByText('textfield');

    //assert
    expect(textFieldElement).toBeInTheDocument();
  });
});
