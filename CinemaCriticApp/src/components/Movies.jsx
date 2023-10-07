import { Modal } from './Modal'
import { useState } from 'react'

const MoviesList = ({ movies = [] }) => {
  const [showOverLay, setShowOverlay] = useState(false)
  const [selectedMovie, setSelectedMovie] = useState(null)

  const handleClick = (movie) => {
    setSelectedMovie(movie)
    setShowOverlay(!showOverLay)
  }
  return (
    <ul className='movies'>
      {
        movies.map(movie => (
          <li className='movie' key={movie.id}>
            <img src={movie.img} alt={movie.title} onClick={() => handleClick(movie)} />
            {
                    showOverLay &&
                    selectedMovie?.id === movie.id
                      ? <Modal movie={movie} setShowOverlay={setShowOverlay} />
                      : ''
                }
            <h4>{movie.title}</h4>
            <p>{movie.release_date}</p>
          </li>
        ))
    }
    </ul>
  )
}

export const Movies = ({ movies = [] }) => {
  const hasMovies = movies?.length > 0
  return (
    hasMovies &&
      <MoviesList movies={movies} />
  )
}
