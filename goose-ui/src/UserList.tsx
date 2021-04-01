import { useEffect, useState } from 'react'
import Dropdown from 'react-dropdown'
import { getCurrentUsername, setCurrentUsername, User } from './api'
import 'react-dropdown/style.css'

interface Props {
  users: readonly User[]
}

const UserList = ({ users }: Props) => {
  const [user, setUser] = useState<User>()
  useEffect(() => {
    getCurrentUsername()
      .then(username => users.find(u => u.username === username))
      .then(setUser)
  }, [users])
  let options = users.map(u => ({ label: u.displayName, value: u.username }))
  return <Dropdown
    value={user?.username}
    options={options}
    onChange={x => {
      setUser(users.find(u => u.username === x.value))
      setCurrentUsername(x.value)
    }}
  />
}
export default UserList