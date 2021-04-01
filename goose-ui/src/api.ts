let baseUrl = process.env.REACT_APP_GOOSE_API_URL

export interface User {
  username: string
  displayName: string
}

export const getUsers = () : Promise<readonly User[]> =>
  fetch(`${baseUrl}/users`)
    .then(response => response.json())

export const getCurrentUsername = () : Promise<string> =>
  fetch(`${baseUrl}/user`)
    .then(response => response.text())

export const setCurrentUsername = (username: string) : Promise<void> =>
  fetch(`${baseUrl}/user`, { method: "PUT", body: username })
    .then(response => { })