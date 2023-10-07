import withoutResults from '../mockups/noResults.json'
import { useRef, useState } from 'react'

export const useMovies = ({ query }) => {
  const previousQuery = useRef(query)
  const [responseMovies, setResponseMovies] = useState([])
  const movies = responseMovies.results

  const mappedMovies = movies?.map(movie => ({
    id: movie.id,
    title: movie.title,
    release_date: movie.release_date,
    img: `https://image.tmdb.org/t/p/w500/${movie.poster_path}`,
    original_language: movie.original_language,
    original_title: movie.original_title,
    popularity: movie.popularity,
    vote_average: movie.vote_average,
    vote_count: movie.vote_count,
    adult: movie.adult

  }))

  const getMovies = () => {
    if (query === previousQuery.current) {
      return
    }
    if (query && query.length > 3) {
      fetch(`https://api.themoviedb.org/3/search/movie?api_key=8f781d70654b5a6f2fa69770d1d115a3&query=${query}`)
        .then(res => res.json())
        .then(json =>
          setResponseMovies(json))

      previousQuery.current = query
    } else {
      setResponseMovies(withoutResults)
    }
  }

  return { movies: mappedMovies, getMovies }
}
