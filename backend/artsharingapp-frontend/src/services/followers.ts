import authAxios from "./authAxios";
import { UserSearchResponse } from "./user";

export async function followUser(userId: number): Promise<boolean> {
  try {
    await authAxios.post(`/follow/${userId}`);
    return true;
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
    return false;
  }
}

export async function unfollowUser(userId: number): Promise<void> {
  try {
    await authAxios.delete(`/unfollow/${userId}`);
  } catch (error: any) {
    const message =
      error?.response?.data?.error ||
      error?.message ||
      "An unknown error occurred.";
    console.error("Error:", message);
  }
}

export async function getFollowers(
  userId: number,
  skip = 0,
  take = 30
): Promise<UserSearchResponse[]> {
  const response = await authAxios.get(`/user/${userId}/followers`, {
    params: { skip, take },
  });
  return response.data;
}

export async function getFollowing(
  userId: number,
  skip = 0,
  take = 30
): Promise<UserSearchResponse[]> {
  const response = await authAxios.get(`/user/${userId}/following`, {
    params: { skip, take },
  });
  return response.data;
}
