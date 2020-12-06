import PropTypes from 'prop-types';
import React, { PureComponent } from 'react';
import Icon from 'Components/Icon';
import { icons } from 'Helpers/Props';
import styles from './HeartRating.css';

class HeartRating extends PureComponent {

  //
  // Render

  render() {

    const {
      ratings,
      hideIcon,
      iconSize
    } = this.props;

    const imdbImage = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABoAAAAOCAYAAAAxDQxDAAAABHNCSVQICAgIfAhkiAAAAh5JREFUOI29lMtPE1EUh797O+08Op2WRyuKCEQTiVF3alwY3bAzLpTEvf+SC/8Cl7IyxsQNETZuTEgkqU8UoqQSCm0onc7MvccFRGxIwEX1LM85OV/O66fkBS5Fbw5jJ9E4DNIsGUqv4cbPHAL3MSL3yKniQCEAOQDpkLq3lCy422giBD1wEIDCIqrloCgjKIDNtsOTV2NUioaRMGWxHlGLUm5favN0qQrAozsNXi4PsfrTRSmYrsXM3WiyWC/xddPl4c0trp3fPQQJGqTiwD4EIE41nxse1SglTjQr6wE/woxK0fDmU0jRtTQ7Dh82PDqx5lQlZakecWGsx8cNn+W1AGsVS+9LzF5pcXli73dfJ46ruevwbj0gM6rPHwWGs8MJSaZo72kyq0BgfavAwkqZt6v9Kz8W5OSEXqqpf/fxC/ZIXCtQ+/WRg/xCXohTRbub+3tQ0bWUg4xuojkzlByXeqIdCwpcw1glpeQbpmtxX2yvp9nadUiNwtGglZAZRZIqrKgjtfoeNKeFcmAo+ZbQM4yEGZUww3UsF0/HfGl4eHmhGqVsbBdotPJMVXtMjvZotPJEgSHvCLUoZSTMjoCEg8sLPcvs1R2KBYtXsIwPJwQFixHFaOmws/vXm+x0cuQd4dxoj4nhBN+1zIx3KfsGrYSZ8e6fHFHy2muClP/Dw8o8lrvA4CUIQOiAPFf7ouo/wJipfyKqOf2NfDz/C1L41ohIYgJDAAAAAElFTkSuQmCC';

    console.log(ratings);

    let rating = ratings.find((r) => {
      return r.origin === 'imdb';
    });

    if (!rating) {
      rating = ratings.find((r) => {
        return r.origin === 'tmdb';
      });
    }

    let ratingString = '0%';

    if (rating) {
      ratingString = rating.origin === 'imdb' ? `${rating.value}` : `${rating.value * 10}%`;
    }

    return (
      <span>
        {
          !hideIcon && rating.origin !== 'imdb' ?
            <Icon
              className={styles.heart}
              name={icons.HEART}
              size={iconSize}
            /> :
            <img
              className={styles.imdb}
              src={imdbImage}
            />
        }

        {ratingString}
      </span>
    );
  }
}

HeartRating.propTypes = {
  ratings: PropTypes.arrayOf(PropTypes.object).isRequired,
  iconSize: PropTypes.number.isRequired,
  hideIcon: PropTypes.bool
};

HeartRating.defaultProps = {
  iconSize: 14
};

export default HeartRating;
