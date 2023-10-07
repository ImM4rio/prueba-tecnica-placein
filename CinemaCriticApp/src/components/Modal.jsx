import { useDispatch } from 'react-redux'
import { addComment } from '../comments/commentsSlice'

export const Modal = ({ movie, setShowOverlay }) => {
  const dispatch = useDispatch()

  const handleClick = () => {
    setShowOverlay(false)
  }

  const handleSubmit = (event, title) => {
    event.preventDefault()
    const { ratingSl, inputTxtArea } = Object.fromEntries(new window.FormData(event.target))
    if (ratingSl && inputTxtArea && title) {
      const newComment = {
        title,
        rating: ratingSl,
        comment: inputTxtArea
      }
      dispatch(addComment(newComment))
      setShowOverlay(false)
    }
  }
  return (

    <div className='modal'>
      <div className='modal-content'>
        <div className='modal-header'>
          <h3 className='modal-title'>{movie.title}</h3>
          <button onClick={handleClick} className='closeBtn'><i className='fa fa-close' /></button>
        </div>
        <div className='modal-body'>
          <div>
            <img src={movie.img} alt={movie.title} />
            <div>
              <h5>{movie.title}</h5>
              <hr />
              <p>Original title: <i>{movie.original_title}</i></p>
              <p>Release date: <i>{movie.release_date}</i></p>
              {
                movie.adult
                  ? (<p>Clasificada para adultos</p>)
                  : (<p>Todos los públicos</p>)
              }
              <p>Original language: <i>{movie.original_language}</i></p>
              <p>Popularity: {movie.popularity}</p>
              <p>Media de votos: {movie.vote_average}</p>
              <p>Puntuación media: {movie.vote_count}</p>
            </div>

          </div>
          <form className='modalForm' onSubmit={event => handleSubmit(event, movie.title)}>
            <h6>Puntuación:</h6>
            <select name='ratingSl'>
              <option value='1'>1</option>
              <option value='2'>2</option>
              <option value='3'>3</option>
              <option value='4'>4</option>
              <option value='5'>5</option>
              <option value='6'>6</option>
              <option value='7'>7</option>
              <option value='8'>8</option>
              <option value='9'>9</option>
              <option value='10'>10</option>
            </select>
            <h6>Comentario:</h6>
            <textarea name='inputTxtArea' placeholder='Qué opinas de la película?' title='Escribe aquí tu comentario sobre la película!' rows={16} cols={32} />
            <button type='submit'>Enviar</button>
          </form>
        </div>
      </div>
    </div>
  )
}
