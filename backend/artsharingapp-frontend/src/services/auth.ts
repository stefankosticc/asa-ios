import axios from "axios";
import authAxios from "./authAxios";

const API_BASE_URL = "http://localhost:5125/api";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
}
export interface SignUpRequest {
  name: string;
  email: string;
  userName: string;
  password: string;
}

export interface User {
  id: number;
  name: string;
  email: string;
  userName: string;
  biography?: string;
  roleId: number;
  roleName?: string;
  followersCount: number;
  followingCount: number;
  profilePhoto: string;
  isFollowedByLoggedInUser: boolean | null;
}

export async function login(request: LoginRequest): Promise<LoginResponse> {
  const response = await axios.post(`${API_BASE_URL}/auth/login`, request);
  return response.data;
}

export async function signUp(request: SignUpRequest): Promise<void> {
  await axios.post(`${API_BASE_URL}/auth/register`, request);
}

export async function getLoggedInUser(): Promise<User> {
  const response = await authAxios.get(`/auth/loggedin-user`);
  return response.data;
}

export async function logout(): Promise<void> {
  const response = await authAxios.post(`${API_BASE_URL}/auth/logout`);
  return response.data;
}
